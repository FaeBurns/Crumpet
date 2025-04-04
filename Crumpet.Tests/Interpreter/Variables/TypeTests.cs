using Crumpet.Interpreter;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;





namespace Crumpet.Tests.Interpreter.Variables;

[TestFixture]
public class TypeTests
{
    [Test]
    public void TestPrimitiveString_Default_Value()
    {
        Variable variable = BuiltinTypeInfo.String.CreateVariable();
        Assert.That(variable.GetValue(), Is.EqualTo(""));
    }

    [Test]
    public void TestPrimitiveInt_Default_Value()
    {
        Variable variable = BuiltinTypeInfo.Int.CreateVariable();
        Assert.That(variable.GetValue(), Is.EqualTo(0));
    }

    [Test]
    public void TestPrimitiveFloat_Default_Value()
    {
        Variable variable = BuiltinTypeInfo.Float.CreateVariable();
        Assert.That(variable.GetValue(), Is.EqualTo(0.0f));
    }

    [Test]
    public void TestPrimitiveBool_Default_Value()
    {
        Variable variable = BuiltinTypeInfo.Bool.CreateVariable();
        Assert.That(variable.GetValue(), Is.EqualTo(false));
    }

    [Test]
    public void TestPrimitive_Sets_Value()
    {
        Variable variable = BuiltinTypeInfo.String.CreateVariable();
        Assert.That(variable.GetValue(), Is.EqualTo(String.Empty));
        variable.SetValue("test");
        Assert.That(variable.GetValue(), Is.EqualTo("test"));
    }

    [Test]
    public void TestObject_Constructs_Simple()
    {
        UserObjectTypeInfo testType = new UserObjectTypeInfo("testType", [], [new FieldInfo("testVar", BuiltinTypeInfo.String), new FieldInfo("testVar2", BuiltinTypeInfo.Int)]);
        Variable variable = testType.CreateVariable();

        Assert.That(variable.GetValue(), Is.TypeOf<UserObjectInstance>());
        Assert.That((variable.GetValue() as UserObjectInstance)!.Fields["testVar"].GetValue(), Is.EqualTo(""));
        Assert.That((variable.GetValue() as UserObjectInstance)!.Fields["testVar2"].GetValue(), Is.EqualTo(0));
        (variable.GetValue() as UserObjectInstance)!.Fields["testVar2"].SetValue(1);
        Assert.That((variable.GetValue() as UserObjectInstance)!.Fields["testVar2"].GetValue(), Is.EqualTo(1));
    }

    [Test]
    public void TestObject_Constructs_Layered()
    {
        UserObjectTypeInfo typeC = new UserObjectTypeInfo("typeC", [], new FieldInfo("fieldC", BuiltinTypeInfo.String));
        UserObjectTypeInfo typeB = new UserObjectTypeInfo("typeB", [], new FieldInfo("fieldB", typeC));
        UserObjectTypeInfo typeA = new UserObjectTypeInfo("typeA", [], new FieldInfo("fieldA", typeB));

        Variable variable = typeA.CreateVariable();

        Assert.That(variable.GetValue(), Is.TypeOf<UserObjectInstance>());

        UserObjectInstance variableA = (UserObjectInstance)variable.GetValue();
        Assert.That(variableA.Fields["fieldA"].Type, Is.EqualTo(typeB));
        Assert.That(variableA.Fields["fieldA"].GetValue(), Is.TypeOf<UserObjectInstance>());

        UserObjectInstance variableB = (UserObjectInstance)variableA.Fields["fieldA"].GetValue()!;
        Assert.That(variableB.Fields["fieldB"].Type, Is.EqualTo(typeC));
        Assert.That(variableB.Fields["fieldB"].GetValue(), Is.TypeOf<UserObjectInstance>());

        UserObjectInstance variableC = (UserObjectInstance)variableB.Fields["fieldB"].GetValue()!;
        Assert.That(variableC.Fields["fieldC"].Type, Is.EqualTo(BuiltinTypeInfo.String));
        Assert.That(variableC.Fields["fieldC"].GetValue(), Is.EqualTo(""));
    }

    [Test]
    public void TestObject_ValueSearcher_FindSingle()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject", new UserObjectTypeInfo("testType", [], new FieldInfo("testVar", BuiltinTypeInfo.String), new FieldInfo("testVar2", BuiltinTypeInfo.Int))));

        ValueSearcher valueSearcher = new ValueSearcher(scope);
        ValueSearchResult testObjectSearchResult = valueSearcher.Find("testObject");

        Assert.That(testObjectSearchResult.DepthReached, Is.EqualTo(1)); // equal to input segments array size
        Assert.That(testObjectSearchResult.Result, Is.Not.Null);
        Assert.That(testObjectSearchResult.Result.Type.TypeName, Is.EqualTo("testType"));
    }

    [Test]
    public void TestObject_ValueSearcher_FindDouble()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject", new UserObjectTypeInfo("testType", [], new FieldInfo("testVar", BuiltinTypeInfo.String), new FieldInfo("testVar2", BuiltinTypeInfo.Int))));

        ValueSearcher valueSearcher = new ValueSearcher(scope);

        ValueSearchResult testVarSearchResult = valueSearcher.Find("testObject.testVar");
        Assert.That(testVarSearchResult.DepthReached, Is.EqualTo(2));
        Assert.That(testVarSearchResult.Result, Is.Not.Null);
        Assert.That(testVarSearchResult.Result.GetValue(), Is.EqualTo(""));
    }

    [Test]
    public void TestObject_ValueSearcher_FindTripple()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject",
            new UserObjectTypeInfo("testType", [],
                [ new FieldInfo("testVar",
                    new UserObjectTypeInfo("testType2", [],
                        [new FieldInfo("testVar2", BuiltinTypeInfo.String)])),
                new FieldInfo("testVar2", BuiltinTypeInfo.Int)])));

        ValueSearcher valueSearcher = new ValueSearcher(scope);

        ValueSearchResult testVarSearchResult = valueSearcher.Find("testObject.testVar.testVar2");
        Assert.That(testVarSearchResult.DepthReached, Is.EqualTo(3));
        Assert.That(testVarSearchResult.Result, Is.Not.Null);
        Assert.That(testVarSearchResult.Result.GetValue(), Is.EqualTo(""));
    }

    [Test]
    public void CloneValueTypes_Int()
    {
        // setup initial value
        Variable initial = BuiltinTypeInfo.Int.CreateVariable();
        Assert.That(initial.GetValue(), Is.EqualTo(default(int)));
        initial.SetValue(10);

        // create copy
        Variable copy = Variable.CreateCopy(initial);
        Assert.That(copy.GetValue(), Is.EqualTo(initial.GetValue()));

        // change value in one
        copy.SetValue(20);
        Assert.That(initial.GetValue(), Is.EqualTo(10));
        Assert.That(copy.GetValue(), Is.EqualTo(20));
    }

    [Test]
    public void CloneValueTypes_String()
    {
        // setup initial value
        Variable initial = BuiltinTypeInfo.String.CreateVariable();
        Assert.That(initial.GetValue(), Is.EqualTo(String.Empty));
        initial.SetValue("test_1");

        // create copy
        Variable copy = Variable.CreateCopy(initial);
        Assert.That(copy.GetValue(), Is.EqualTo(initial.GetValue()));

        // change value in one
        copy.SetValue("test_2");
        Assert.That(initial.GetValue(), Is.EqualTo("test_1"));
        Assert.That(copy.GetValue(), Is.EqualTo("test_2"));
    }

    [Test]
    public void ConvertType_IntToFloat()
    {
        // TODO implement
    }
}