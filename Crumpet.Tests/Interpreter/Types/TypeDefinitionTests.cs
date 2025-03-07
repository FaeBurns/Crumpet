using Crumpet.Interpreter;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Crumpet.Language.Nodes;
using Lexer;
using Parser;
using Parser.Nodes;

namespace Crumpet.Tests.Interpreter.Types;

[TestFixture]
public class TypeDefinitionTests
{
    public void RunExampleFile(string filename, IEnumerable<TypeInfo> expectedTypes)
    {
        TestProgramFile(Path.Combine("Examples//", filename) + ".crm", expectedTypes);
    }

    public void TestProgramFile(string path, IEnumerable<TypeInfo> expectedTypes)
    {
        TestProgram(File.ReadAllText(path), expectedTypes);
    }

    private void TestProgram(string programText, IEnumerable<TypeInfo> expectedTypes)
    {
        NonTerminalNode programRoot = BuildProgram(programText);

        TreeWalkingInterpreter interpreter = new TreeWalkingInterpreter(programRoot);

        foreach (TypeInfo expected in expectedTypes)
        {
            TypeInfo? resolved = interpreter.TypeResolver.ResolveType(expected.TypeName);

            Assert.That(resolved, Is.Not.Null);
            Assert.That(resolved.TypeName, Is.EqualTo(expected.TypeName));
            Assert.That(resolved, Is.TypeOf(expected.GetType()));

            if (resolved is UserObjectTypeInfo resolvedUserObjectType && expected is UserObjectTypeInfo expectedUserObjectType)
            {
                Assert.That(resolvedUserObjectType.Fields.Length, Is.EqualTo(expectedUserObjectType.Fields.Length));
                foreach (FieldInfo field in resolvedUserObjectType.Fields)
                {
                    FieldInfo? expectedField = expectedUserObjectType.Fields.FirstOrDefault(x => x.Name == field.Name);
                    Assert.That(expectedField, Is.Not.Null);
                    Assert.That(field.Type.TypeName, Is.EqualTo(expectedField.Type.TypeName));
                    Assert.That(field.VariableModifier, Is.EqualTo(expectedField.VariableModifier));
                }
            }
        }
    }

    private NonTerminalNode BuildProgram(string text)
    {
        ILexer<CrumpetToken> lexer = new Lexer<CrumpetToken>(text, CrumpetToken.WHITESPACE, CrumpetToken.NEWLINE, CrumpetToken.COMMENT);
        IEnumerable<Token<CrumpetToken>> tokens = lexer.Tokenize();

        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();

        NodeTypeTree<CrumpetToken> nodeTree = new NodeTypeTree<CrumpetToken>(registry, typeof(RootNonTerminalNode));

        NodeWalkingParser<CrumpetToken,RootNonTerminalNode> parser = new NodeWalkingParser<CrumpetToken, RootNonTerminalNode>(registry, nodeTree);

        ParseResult<CrumpetToken, RootNonTerminalNode> result = parser.ParseToRoot(tokens);

        Assert.That(result.Root, Is.Not.Null);

        return result.Root;
    }

    [Test]
    public void TestTypeResolution()
    {
        UserObjectTypeInfo typeA = new UserObjectTypeInfo("TypeA",
            new FieldInfo("a", new BuiltinTypeInfo<int>()),
            new FieldInfo("b", new BuiltinTypeInfo<int>()),
            new FieldInfo("c", new BuiltinTypeInfo<int>()),
            new FieldInfo("child", null!));

        UserObjectTypeInfo typeB = new UserObjectTypeInfo("TypeB",
            new FieldInfo("parent", typeA, VariableModifier.POINTER),
            new FieldInfo("a", new BuiltinTypeInfo<string>()),
            new FieldInfo("b", new BuiltinTypeInfo<float>()));

        typeA.Fields[3].Type = typeB;

        RunExampleFile("Interpreter/typeResolution", [typeA, typeB]);
    }
}