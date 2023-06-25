using ISTB.Framework.Attributes.BaseAttributes;
using System.Reflection;

namespace ISTB.Framework.Executors.Storages.TargetMethod.Models
{
    public class TargetMethodInfo
    {
        public MethodInfo MethodInfo { get; set; } = default!;
        public ICollection<TargetAttribute> TargetAttributes { get; set; } = default!;
    }
}
