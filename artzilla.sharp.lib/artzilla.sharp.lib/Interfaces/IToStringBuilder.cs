using System.Text;

namespace ArtZilla.Net.Core.Interfaces; 

/// alternative for method <see cref="object.ToString"/>
public interface IToStringBuilder {
	/// print current object to string builder
	void ToStringBuilder(StringBuilder sb);
}