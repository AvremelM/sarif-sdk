// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type Address for sorting.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "2.1.0.0")]
    internal sealed class AddressComparer : IComparer<Address>
    {
        internal static readonly AddressComparer Instance = new AddressComparer();

        public int Compare(Address left, Address right)
        {
            int compareResult = 0;

            // TryReferenceCompares is an autogenerated extension method
            // that will properly handle the case when 'left' is null.
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = left.AbsoluteAddress.CompareTo(right.AbsoluteAddress);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.RelativeAddress.CompareTo(right.RelativeAddress);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Length.CompareTo(right.Length);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Kind, right.Kind);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Name, right.Name);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.FullyQualifiedName, right.FullyQualifiedName);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.OffsetFromParent.CompareTo(right.OffsetFromParent);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Index.CompareTo(right.Index);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.ParentIndex.CompareTo(right.ParentIndex);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Properties.DictionaryCompares(right.Properties, SerializedPropertyInfoComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            return compareResult;
        }
    }
}