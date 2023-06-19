using ISTB.Framework.Attributes.BaseAttributes;
using System.Reflection;

namespace ISTB.Framework.Executors.Storages.TargetMethodRoutes
{
    public class TargetMethodInfo
    {
        public MethodInfo MethodInfo { get; set; } = default!;
        public ICollection<TargetAttribute> TargetAttributes { get; set; } = default!;
    }
}
