
namespace challenge.Models
{
    public class BaseModel
    {
        public virtual bool IsError { get; set; } = false;
        public virtual string ErrorMessage { get; set; }
    }
}
