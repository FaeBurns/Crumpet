using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;




namespace Crumpet.Tests.Interpreter;

[TestFixture]
public class VariableTests
{
    [Test]
    public void PrimitiveTypes()
    {

    }

    [Test]
    public void VariableCollection_Create_Valid()
    {
        VariableCollection collection = new VariableCollection();

        collection.Create(new VariableInfo("testVar", BuiltinTypeInfo.String));
        Assert.That(collection.Has("testVar"));
        Assert.That(collection.CheckType("testVar", BuiltinTypeInfo.String));
        Assert.That(collection.GetVariable("testVar").GetValue(), Is.EqualTo(String.Empty));
    }

    [Test]
    public void VariableCollection_Saves_Changes()
    {
        VariableCollection collection = new VariableCollection();
        collection.Create(new VariableInfo("testVar", BuiltinTypeInfo.String));

        Assert.That(collection.GetVariable("testVar").GetValue(), Is.EqualTo(String.Empty));
        collection.GetVariable("testVar").SetValue("testValue");
        Assert.That(collection.GetVariable("testVar").GetValue(), Is.EqualTo("testValue"));
    }

    [Test]
    public void AssignVariable_Copy()
    {
        // setup variables
        Variable initial = BuiltinTypeInfo.Int.CreateVariable();
        Variable copy = BuiltinTypeInfo.Int.CreateVariable();

        Assert.That(initial.GetValue(), Is.EqualTo(default(int)));
        initial.SetValue(10);
        copy.SetValue(initial);

        // create copy
        Assert.That(copy.GetValue(), Is.EqualTo(initial.GetValue()));

        // change value in one and test it does not propagate to the other
        copy.SetValue(20);
        Assert.That(initial.GetValue(), Is.EqualTo(10));
        Assert.That(copy.GetValue(), Is.EqualTo(20));
    }

    [Test]
    public void AssignVariable_Pointer()
    {
        // setup variables
        Variable initial = BuiltinTypeInfo.Int.CreateVariable();
        Variable pointer = Variable.CreatePointer(BuiltinTypeInfo.Int, initial);

        Assert.That(initial.GetValue(), Is.EqualTo(default(int)));
        initial.SetValue(10);
        pointer.SetValue(initial);

        // create copy
        Assert.That(pointer.DereferenceToValue(), Is.EqualTo(initial.GetValue()));

        // change value in one and test it does not propagate to the other
        pointer.GetValue<Variable>().SetValue(20);
        Assert.That(initial.GetValue(), Is.EqualTo(20));
        Assert.That(pointer.DereferenceToValue(), Is.EqualTo(20));

        initial.SetValue(30);
        Assert.That(initial.GetValue(), Is.EqualTo(30));
        Assert.That(pointer.DereferenceToValue(), Is.EqualTo(30));
    }
}