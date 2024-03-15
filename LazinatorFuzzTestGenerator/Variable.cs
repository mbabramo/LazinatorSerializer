namespace LazinatorFuzzTestGenerator
{
    debug; // think about separating types from values, so we can represent values as types. This would be true for classes, subclasses, etc. We want to have something to represent the state of each of these. 
    public class Variable
    {
        public IObjectContents Contents { get; set; }
        public int VariableNumber { get; set; }
        public string VariableName => "v" + VariableNumber;

        public Variable(IObjectContents contents, int variableNumber)
        {
            Contents = contents;
            VariableNumber = variableNumber;
        }

        public string GetAssignmentStatement()
        {
            return VariableName + " = " + Contents.CodeToGetValue + ";";
        }
    }
}
