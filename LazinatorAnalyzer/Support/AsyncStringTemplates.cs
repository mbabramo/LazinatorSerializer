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
        /// Creates a main and an async version of code, if mayBeAsync is true, or else just the main version.
        /// </summary>
        /// <param name="mayBeAsync"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public string NotAsyncAndMaybeAsync(bool mayBeAsync, string contents) => mayBeAsync ? CreateForBlock("async", 0, 2, MaybeAsyncMainBlock(mayBeAsync, contents)) : contents;

        /// <summary>
        /// Create code that will be conditional based on whether this is an async block or not. 
        /// </summary>
        /// <param name="mayBeAsync"></param>
        /// <param name="asyncVersion"></param>
        /// <param name="nonasyncVersion"></param>
        /// <returns></returns>
        public string MaybeAsyncConditional(bool mayBeAsync, string asyncVersion, string nonasyncVersion = "") => mayBeAsync ? (CreateIfBlock("async", "0", nonasyncVersion) + CreateIfBlock("async", "1", asyncVersion)) : nonasyncVersion;

        /// <summary>
        /// Creates the main block for storing async code. This will insert the word async before the code if the code calls await.
        /// To determine whether the code calls await, the "awaitcalled" variable is checked, and this is reset at the end of the call.
        /// </summary>
        /// <param name="mayBeAsync"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private string MaybeAsyncMainBlock(bool mayBeAsync, string contents) => mayBeAsync ? CreateReprocessBlock(MaybeAsyncWord_async(mayBeAsync) + contents) + CreateSetVariableBlock("awaitCalled", null) : contents;

        public string MaybeAsyncBlockReturnType(bool mayBeAsync, string ordinaryReturnType) => MaybeAsyncConditional(mayBeAsync, MaybeAsyncReturnType(ordinaryReturnType), ordinaryReturnType); 

        public string MaybeAsyncWordAwait(bool mayBeAsync) => MaybeAsyncConditional(mayBeAsync, "await " + CreateSetVariableBlock("awaitcalled", "1"));

        public string MaybeAsyncWordAsync(bool mayBeAsync) => MaybeAsyncConditional(mayBeAsync, "Async");
        public string MaybeAsyncWord_async(bool mayBeAsync) => MaybeAsyncConditional(mayBeAsync, CreateIfBlock("awaitcalled", "1", "async "));

        private string MaybeAsyncReturnType(string ordinaryReturnType) => ordinaryReturnType == "void" ? TaskKeyword : $"{TaskKeyword}<{ordinaryReturnType}>";

        public string MaybeAsyncReturnValue(bool mayBeAsync, string ordinaryReturnValue) => MaybeAsyncConditional(mayBeAsync, CreateIfBlock("awaitcalled", "0", $"{TaskKeyword}.FromResult({ordinaryReturnValue})") + CreateIfBlock("awaitcalled", "1", ordinaryReturnValue), ordinaryReturnValue);
        public string MaybeAsyncVoidReturn(bool mayBeAsync) => MaybeAsyncConditional(mayBeAsync, CreateIfBlock("awaitcalled", "0", $"return {TaskKeyword}.CompletedTask;"));

    }
}
