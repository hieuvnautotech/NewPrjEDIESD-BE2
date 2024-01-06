namespace NewPrjESDEDIBE.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowAllAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class JSGenerateAttribute : Attribute
    {
    }

}
