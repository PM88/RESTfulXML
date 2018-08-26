using System.Data.Entity.ModelConfiguration;
using RESTfulXML_1.Models;

namespace RESTfulXML_1.DataAccess.EntityConfigurations
{
    public class RequestConfiguration : EntityTypeConfiguration<Request>
    {
        public RequestConfiguration()
        {
            HasKey(r => new { r.Index, r.Name, r.Date });

            Property(r => r.Index)
                .HasColumnName("Ix")
                .HasParameterName("Ix");

            Property(r => r.Name)
                .HasParameterName("Name");

            Property(r => r.Visits)
                .IsOptional()
                .HasParameterName("Visits");

            Property(r => r.Date)
                .HasParameterName("Date");
        }
    }
}