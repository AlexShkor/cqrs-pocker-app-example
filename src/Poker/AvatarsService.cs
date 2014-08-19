using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Poker
{
    public class AvatarsService
    {
        const string Root = "/assets/avatars/";

        public IEnumerable<AvatarInfo> GetAll()
        {
            return Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "avatars"))
                .Select(x => new FileInfo(x))
                .OrderBy(x=> x.Name)
                .Select(x => new AvatarInfo
                {
                    Id = x.Name,
                    Url = Root + x.Name
                });
        }

        public static string GetUrlById(string avatarId)
        {
            return Root + (avatarId ?? "sample_avatar.jpg");
        }
    }

    public class AvatarInfo
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }
}