using System.Data;

namespace SWSoft.Reflector
{
    public enum CodeType
    {
        CSharp, SQL, Java
    }

    public partial class Code
    {
        public static CodeFile Create(CodeFile codefile, DataTable table, CodeType codetype)
        {
            switch (codetype)
            {
                case CodeType.CSharp: return Create(codefile, table);
                case CodeType.SQL:
                    break;
                case CodeType.Java:
                    break;
            }
            return null;
        }
    }
}
