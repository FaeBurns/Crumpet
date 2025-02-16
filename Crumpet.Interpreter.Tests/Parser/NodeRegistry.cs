using Crumpet.Interpreter.Parser;
using Crumpet.Language;

namespace Crumpet.Interpreter.Tests.Parser;

[TestFixture]
public class NodeRegistry
{
    private ASTNodeRegister RegisterNodes()
    {
        ASTNodeRegister register = new ASTNodeRegister();
        register.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();
        return register;
    }

    [Test]
    public void TestRegisterFactoryCollection()
    {
        RegisterNodes();
    }

    [TestCase("boolLiteral")]
    [TestCase("floatLiteral")]
    [TestCase("identifier")]
    [TestCase("intLiteral")]
    [TestCase("stringLiteral")]
    public void TestRegisterTerminal(string name, int variantCount = 1)
    {
        ASTNodeRegister register = RegisterNodes();
        Assert.That(register.GetTerminalDefinitions(name).Count(), Is.EqualTo(variantCount));
    }

    [TestCase("root_program")]
    [TestCase("declaration", 2)]
    [TestCase("functionDeclaration")]
    [TestCase("parameterList")]
    [TestCase("parameter")]
    [TestCase("typeDeclaration")]
    [TestCase("typeDeclarationField")]
    [TestCase("statementBody")]
    [TestCase("statement", 5)]
    [TestCase("flowStatement")]
    [TestCase("ifStatement")]
    [TestCase("iterationStatement")]
    [TestCase("initializationStatement")]
    [TestCase("type")]
    [TestCase("expression")]
    [TestCase("unaryExpression")]
    [TestCase("expressionWithPostfix")]
    [TestCase("primaryExpression", 3)]
    [TestCase("assignmentExpression", 2)]
    [TestCase("argumentExpressionList")]
    [TestCase("literalConstant", 4)]
    [TestCase("orExpression")]
    [TestCase("andExpression")]
    [TestCase("exclusiveOrExpression")]
    [TestCase("equalityExpression")]
    [TestCase("relationExpression")]
    [TestCase("sumExpression")]
    [TestCase("multExpression")]
    public void TestRegisterNonTerminal(string name, int variantCount = 1)
    {
        ASTNodeRegister register = RegisterNodes();
        Assert.That(register.GetNonTerminalDefinitions(name).Count(), Is.EqualTo(variantCount));
    }
}