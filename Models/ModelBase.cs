using System;
using System.ComponentModel.DataAnnotations;

namespace SokoGrump.Models
{
    public abstract class ModelBase : IEquatable<ModelBase>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [StringLength(300, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="ModelBase"/> is equal to the current <see cref="ModelBase"/>.
        /// </summary>
        /// <param name="other">The <see cref="ModelBase"/> to compare with the current <see cref="ModelBase"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="ModelBase"/> is equal to the current
        /// <see cref="ModelBase"/>; otherwise, <c>false</c>.</returns>
        public virtual bool Equals(ModelBase other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Id, other.Id) &&
                   string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="ModelBase"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="ModelBase"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="ModelBase"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ModelBase)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="ModelBase"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id is not null ? Id.GetHashCode() : 0) * 397) ^
                       (Name is not null ? Name.GetHashCode() : 0) ^
                       (Description is not null ? Description.GetHashCode() : 0);
            }
        }
    }
}
