using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LazinatorAnalyzer.Support
{
    public class AsyncStringTemplates : StringTemplates
    {
        public string TaskKeyword = "ValueTask";

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
        public string MaybeAsyncAndNot_Begin => MayBeAsync ? CreateForBlock_Begin("async", 0, 2) + MaybeAsyncMainBlock_Begin : "";
        public string MaybeAsyncAndNot_End => MayBeAsync ? "\r\n" + CreateEndCommand() + MaybeAsyncMainBlock_End : "";

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
        /// To determine whether the code calls await, the "awaitcalled" variable is checked, and this is reset at the end of the call.
        /// </summary>
        /// <param name="mayBeAsync"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private string MaybeAsyncMainBlock(string contents) => MayBeAsync ? CreateReprocessBlock(MaybeAsyncWord_async() + CreateSetVariableBlock("awaitcalled", "0") + contents) + CreateSetVariableBlock("awaitcalled", null) : contents;
        private string MaybeAsyncMainBlock_Begin => MayBeAsync ? CreateReprocessBlock_BeginOnly() + MaybeAsyncWord_async() + CreateSetVariableBlock("awaitcalled", "0") : "";
        private string MaybeAsyncMainBlock_End => CreateSetVariableBlock("awaitcalled", null) + CreateEndCommand();

        public string MaybeAsyncReturnType(string ordinaryReturnType) => MaybeAsyncConditional(MaybeAsyncReturnTypeWrapper(ordinaryReturnType), ordinaryReturnType); 

        public string MaybeAsyncWordAwait() => MaybeAsyncConditional("await " + CreateSetVariableBlock("awaitcalled", "1"));

        public string MaybeAsyncWordAsync() => MaybeAsyncConditional("Async");
        public string MaybeAsyncWord_async() => MaybeAsyncConditional(CreateIfBlock("awaitcalled", "1", "async "));

        private string MaybeAsyncReturnTypeWrapper(string ordinaryReturnType) => ordinaryReturnType == "void" ? TaskKeyword : $"{TaskKeyword}<{ordinaryReturnType}>";

        public string MaybeAsyncReturnValue(string ordinaryReturnValue) => MaybeAsyncConditional(CreateIfBlock("awaitcalled", "0", $"{TaskKeyword}.FromResult({ordinaryReturnValue})") + CreateIfBlock("awaitcalled", "1", ordinaryReturnValue), ordinaryReturnValue);
        public string MaybeAsyncVoidReturn(bool isAtEndOfMethod) => MaybeAsyncConditional(CreateIfBlock("awaitcalled", "0", $"return {TaskKeyword}.CompletedTask;") + (isAtEndOfMethod ? "" : CreateIfBlock("awaitcalled", "1", $"return;")));

    }
}
