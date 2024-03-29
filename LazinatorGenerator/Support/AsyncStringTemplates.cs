﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LazinatorGenerator.Support
{
    public class AsyncStringTemplates : StringTemplates
    {
        public string TaskKeyword = "ValueTask";

        // The following options can be used to debug non matching uses of MaybeAsyncAndNot, by inserting numbered comments into the code.
        public bool MatchMaybeAsyncAndNotNumerically = false;
        public int MaybeAsyncBeginIndex = 0;
        public int MaybeAsyncEndIndex = 0;
        public string MaybeAsyncIndexStringForMatching(bool begin) => MatchMaybeAsyncAndNotNumerically ? $"/*{(begin ? MaybeAsyncBeginIndex++ : MaybeAsyncEndIndex++)} */" : "";

        /// <summary>
        /// If this is false, then the code will always be added in its synchronous form. If true, then it will be added in its
        /// asynchronous form or, if NotAsyncAndMaybeAsync or MaybeAsyncConditional is used, then in both forms.
        /// </summary>
        public bool MayBeAsync = true;

        /// <summary>
        /// Creates a main and an async version of code, if mayBeAsync is true, or else just the main version.
        /// </summary>
        /// <param name="mayBeAsync"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public string MaybeAsyncAndNot(string contents) => MayBeAsync ? CreateForBlock("async", 0, 2, MaybeAsyncMainBlock(contents + "\r\n")) : contents;
        public string MaybeAsyncAndNot_Begin => MayBeAsync ? MaybeAsyncIndexStringForMatching(true) + CreateForBlock_Begin("async", 0, 2) + MaybeAsyncMainBlock_Begin : "";
        public string MaybeAsyncAndNot_End => MayBeAsync ? MaybeAsyncIndexStringForMatching(false) + "\r\n" + CreateEndCommand("reprocess") + MaybeAsyncMainBlock_End : "";
        /// <summary>
        /// a block that should not be async. This is useful as a quick way of converting a maybe-async-and-not block into non-async code, without eliminating
        /// all of the inner code checking for asyncness.
        /// </summary>
        public string NotAsync_Begin => MayBeAsync ? MaybeAsyncIndexStringForMatching(true) + CreateForBlock_Begin("async", 0, 1) + MaybeAsyncMainBlock_Begin : "";
        public string NotAsync_End => MayBeAsync ? MaybeAsyncIndexStringForMatching(false) + "\r\n" + CreateEndCommand("reprocess") + MaybeAsyncMainBlock_End : "";

        public string NotAsync(string contents) => MayBeAsync ? CreateForBlock("async", 0, 1, MaybeAsyncMainBlock(contents + "\r\n")) : contents;


        /// <summary>
        /// Create code that will be conditional based on whether this is an async block or not. 
        /// </summary>
        /// <param name="mayBeAsync"></param>
        /// <param name="asyncVersion"></param>
        /// <param name="nonasyncVersion"></param>
        /// <returns></returns>
        public string MaybeAsyncConditional(string asyncVersion, string nonasyncVersion = "") => MayBeAsync ? (CreateIfBlock("async", "0", nonasyncVersion) + CreateIfBlock("async", "1", asyncVersion)) : nonasyncVersion;

        /// <summary>
        /// Creates the main block for storing async code. This will insert the word async before the code if the code calls await.
        /// To determine whether the code calls await, the "asyncused" variable is checked, and this is reset at the end of the call.
        /// </summary>
        /// <param name="mayBeAsync"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private string MaybeAsyncMainBlock(string contents) => MayBeAsync ? CreateReprocessBlock(MaybeAsyncWord_async() + InitializeAsyncNotUsed() + contents, 0)  : contents;
        private string MaybeAsyncMainBlock_Begin => MayBeAsync ? CreateReprocessBlock_BeginOnly(0) + MaybeAsyncWord_async() + InitializeAsyncNotUsed() : "";
        private string MaybeAsyncMainBlock_End => CreateEndCommand("for");

        public string MaybeAsyncReturnType(string ordinaryReturnType) => MaybeAsyncConditional(MaybeAsyncReturnTypeWrapper(ordinaryReturnType), ordinaryReturnType);

        public string MaybeAsyncWordAwait() => MaybeAsyncConditional(AwaitAndNoteAsyncUsed());
        public string AwaitAndNoteAsyncUsed() => "await " + NoteAsyncUsed();

        public string InitializeAsyncNotUsed() => CreateSetVariableBlock("asyncused", "0");
        public string NoteAsyncUsed() => CreateSetVariableBlock("asyncused", "1");

        public string MaybeAsyncWordAsync() => MaybeAsyncConditional("Async");
        public string MaybeAsyncWord_async() => MaybeAsyncConditional(OnlyIfAsyncUsedInBlock("async "));
        public string MaybeIAsyncEnumerable() => MaybeAsyncConditional("IAsyncEnumerable" + NoteAsyncUsed(), "IEnumerable");
        public string MaybeInIfNotAsync() => MaybeAsyncConditional("", "in ");

        private string MaybeAsyncReturnTypeWrapper(string ordinaryReturnType) => ordinaryReturnType == "void" ? TaskKeyword : $"{TaskKeyword}<{ordinaryReturnType}>";

        public string MaybeAsyncReturnValue(string ordinaryReturnValue) => MaybeAsyncConditional(OnlyIfAsyncNotUsedInBlock($"{TaskKeyword}.FromResult({ordinaryReturnValue})") + OnlyIfAsyncUsedInBlock(ordinaryReturnValue), ordinaryReturnValue);
        public string MaybeAsyncVoidReturn(bool isAtEndOfMethod) => MaybeAsyncConditional(OnlyIfAsyncNotUsedInBlock($@"return {TaskKeyword}.CompletedTask;
                    ") + (isAtEndOfMethod ? "" : OnlyIfAsyncUsedInBlock($@"return;
                    ")), isAtEndOfMethod ? "" : $@"return;
                    ");

        public string OnlyIfAsyncUsedInBlock(string text) => CreateReprocessBlock(CreateIfBlock("asyncused", "1", text), 1);
        public string OnlyIfAsyncNotUsedInBlock(string text) => CreateReprocessBlock(CreateIfBlock("asyncused", "0", text), 1);

    }
}
