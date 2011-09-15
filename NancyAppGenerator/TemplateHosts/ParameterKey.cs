using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyAppGenerator.TemplateHosts
{
    struct ParameterKey : IEquatable<ParameterKey>
    {
        public ParameterKey(string processorName, string directiveName, string parameterName)
        {
            this.processorName = processorName ?? "";
            this.directiveName = directiveName ?? "";
            this.parameterName = parameterName ?? "";
            unchecked
            {
                hashCode = this.processorName.GetHashCode()
                    ^ this.directiveName.GetHashCode()
                    ^ this.parameterName.GetHashCode();
            }
        }

        string processorName, directiveName, parameterName;
        int hashCode;

        public override bool Equals(object obj)
        {
            return obj != null && obj is ParameterKey && Equals((ParameterKey)obj);
        }

        public bool Equals(ParameterKey other)
        {
            return processorName == other.processorName && directiveName == other.directiveName && parameterName == other.parameterName;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
