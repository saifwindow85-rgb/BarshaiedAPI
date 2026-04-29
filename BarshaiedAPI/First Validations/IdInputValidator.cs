using System.ComponentModel.DataAnnotations;

namespace BarshaiedAPI.First_Validations
{
    public class IdInputValidator
    {
        [Range(1,int.MaxValue,ErrorMessage = "Invalid Id Format!/Id Must Be > 0")]
        public int Id { get; set; }
    }
}
