using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class Chat
{
    public long Id { get; set; }

    public string Message { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public long UserChatId { get; set; }

    public virtual UserChat UserChat { get; set; } = null!;
}
