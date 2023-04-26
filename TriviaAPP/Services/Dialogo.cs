using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaAPP.Services
{
    public interface IDialogService
    {
        Task DisplayAlert(string title, string message, string acceptButton);
    }
}
