using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.EFCore.Entities
{
    /// <summary>
    /// Entity class for file descriptor.
    /// </summary>
    public class FileDescriptorEntity
    {
        [Key]
        [MaxLength(400)]
        public string FullPath { get; private set; }

        [Required]
        public ChangeInfoType ChangeInfoType { get; private set; }
                
        [MaxLength(255)]
        public string? Name { get; private set; }
        
        [Required]
        public int Age { get; set; }

        public FileDescriptorEntity(ChangeInfoType changeInfoType, string fullPath, string? name)
        {
            ChangeInfoType = changeInfoType;
            FullPath = fullPath;
            Name = name;
            Age = 0;
        }
    }
}
