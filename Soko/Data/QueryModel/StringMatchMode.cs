using System;

namespace Soko.Data.QueryModel
{
    [Serializable]
    public enum StringMatchMode
    {
        Exact,
        Anywhere,
        End,
        Start
    }
}
