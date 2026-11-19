using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WRFPractice.Models;

namespace WRFPractice.Services;

public interface IAiService
{
    Task<string> SendMessageAsync(
        IReadOnlyList<ChatMessage> history,
        AppSettings settings,
        CancellationToken cancellationToken = default);
}
