using Crumpet.Interpreter.Interpreter.Variables;
using Crumpet.Interpreter.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Tests.Interpreter;

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
        
        collection.Create(new VariableInfo("testVar", new BuiltinTypeInfo<string>()));
        Assert.That(collection.Has("testVar"));
        Assert.That(collection.CheckType("testVar", new BuiltinTypeInfo<string>()));
        Assert.That(collection.GetReference("testVar").Value, Is.EqualTo(String.Empty));
    }

    [Test]
    public void VariableCollection_Saves_Changes()
    {
        VariableCollection collection = new VariableCollection();
        collection.Create(new VariableInfo("testVar", new BuiltinTypeInfo<string>()));
        
        Assert.That(collection.GetReference("testVar").Value, Is.EqualTo(String.Empty));
        collection.GetReference("testVar").Value = "testValue";
        Assert.That(collection.GetReference("testVar").Value, Is.EqualTo("testValue"));
    }
}