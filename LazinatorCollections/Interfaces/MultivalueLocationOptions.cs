﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// The location at which to find, insert, or remove an item, if duplicates are allowed. For insertions, an insertion will replace an existing item unless BeforeFirst or AfterLast is used.
    /// </summary>
    public enum MultivalueLocationOptions
    {
        Any,
        InsertBeforeFirst,
        First,
        Last,
        InsertAfterLast
    }
}