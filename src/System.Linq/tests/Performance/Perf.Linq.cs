﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Linq.Tests
{
    public class Perf_Linq
    {
        #region Helper Methods

        /// <summary>
        /// Provides TestInfo data to xunit performance tests
        /// </summary>
        public static IEnumerable<object[]> IterationSizeWrapperData()
        {
            int[] iterations = { 1000 };
            int[] sizes = { 100 };
            foreach (int iteration in iterations)
                foreach (int size in sizes)
                {
                    yield return new object[] { size, iteration, Perf_LinqTestBase.WrapperType.NoWrap };
                    yield return new object[] { size, iteration, Perf_LinqTestBase.WrapperType.IEnumerable };
                    yield return new object[] { size, iteration, Perf_LinqTestBase.WrapperType.IReadOnlyCollection };
                    yield return new object[] { size, iteration, Perf_LinqTestBase.WrapperType.ICollection };
                }
        }

        /// <summary>
        /// Provides TestInfo data to xunit performance tests
        /// </summary>
        public static IEnumerable<object[]> IterationSizeWrapperDataNoWrapper()
        {
            int[] iterations = { 1000 };
            int[] sizes = { 100 };
            foreach (int iteration in iterations)
                foreach (int size in sizes)
                    yield return new object[] { size, iteration };
        }

        private class BaseClass
        {
            public int Value;
        }
        private class ChildClass : BaseClass
        {
            public int ChildValue;
        }

        #endregion

        #region Perf Tests

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void Select(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Select(o => o + 1));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void SelectSelect(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Select(o => o + 1).Select(o => o - 1));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void Where(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Where(o => o >= 0));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void WhereWhere(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Where(o => o >= 0).Where(o => o >= -1));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void WhereSelect(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Where(o => o >= 0).Select(o => o + 1));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void Cast_ToBaseClass(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Func<IEnumerable<ChildClass>, IEnumerable<BaseClass>> linqApply = col => col.Cast<BaseClass>();
            ChildClass val = new ChildClass() { Value = 1, ChildValue = 2 };
            Perf_LinqTestBase.Measure<ChildClass, BaseClass>(10, 5, val, wrapType, linqApply);
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void Cast_SameType(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Func<IEnumerable<int>, IEnumerable<int>> linqApply = col => col.Cast<int>();
            int val = 1;
            Perf_LinqTestBase.Measure<int, int>(10, 5, val, wrapType, linqApply);
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void OrderBy(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.OrderBy(o => -o));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void OrderByDescending(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.OrderByDescending(o => -o));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void OrderByThenBy(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.OrderBy(o => -o).ThenBy(o => o));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperDataNoWrapper")]
        public void Range(int size, int iteration)
        {
            Perf_LinqTestBase.Measure<int>(1, iteration, Perf_LinqTestBase.WrapperType.NoWrap, col => Enumerable.Range(0, size));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperDataNoWrapper")]
        public void Repeat(int size, int iteration)
        {
            Perf_LinqTestBase.Measure<int>(1, iteration, Perf_LinqTestBase.WrapperType.NoWrap, col => Enumerable.Repeat(0, size));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void Reverse(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Reverse());
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void Skip(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Skip(1));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void Take(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Take(size - 1));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void SkipTake(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            Perf_LinqTestBase.Measure<int>(size, iteration, wrapType, col => col.Skip(1).Take(size - 2));
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void ToArray(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            int[] array = Enumerable.Range(0, size).ToArray();
            Perf_LinqTestBase.MeasureMaterializationToArray<int>(Perf_LinqTestBase.Wrap(array, wrapType), iteration);
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void ToList(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            int[] array = Enumerable.Range(0, size).ToArray();
            Perf_LinqTestBase.MeasureMaterializationToList<int>(Perf_LinqTestBase.Wrap(array, wrapType), iteration);
        }

        [Benchmark]
        [MemberData("IterationSizeWrapperData")]
        public void ToDictionary(int size, int iteration, Perf_LinqTestBase.WrapperType wrapType)
        {
            int[] array = Enumerable.Range(0, size).ToArray();
            Perf_LinqTestBase.MeasureMaterializationToDictionary<int>(Perf_LinqTestBase.Wrap(array, wrapType), iteration);
        }

        #endregion
    }
}
