﻿namespace GitChat.Shared
{
    public class RegisterResultDTO
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
