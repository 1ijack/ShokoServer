﻿using JMMContracts.PlexAndKodi;
using JMMServer.Entities;
using System.Collections.Generic;

namespace JMMServer.API.Model.common
{
    public class Episode
    {
        public int id { get; set; }
        public string type { get; set; }
        public ArtCollection art { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string year { get; set; }
        public string air { get; set; }
        public string rating { get; set; }
        public int view { get; set; }
        public int eptype { get; set; }
        public int epnumber { get; set; }
        public List<Role> roles { get; set; }
        public List<Tag> tags { get; set; }
        public List<RawFile> files { get; set; }

        public Episode()
        {

        }

        public Episode(AnimeEpisode aep, int uid)
        {
            art = new ArtCollection();
            roles = new List<Role>();
            tags = new List<Tag>();

            id = aep.AnimeEpisodeID;
            type = aep.EpisodeTypeEnum.ToString();
            title = aep.PlexContract?.Title;
            summary = aep.PlexContract?.Summary;
            year = aep.PlexContract?.Year;
            air = aep.PlexContract?.AirDate.ToString();
            rating = aep.PlexContract?.Rating;

            JMMContracts.Contract_AnimeEpisode cae = aep.GetUserContract(uid);

            view = cae.IsWatched;
            epnumber = cae.EpisodeNumber;
            eptype = cae.EpisodeType;

            if (aep.PlexContract?.Roles != null)
            {
                foreach (RoleTag rl in aep.PlexContract?.Roles)
                {
                    Role n_rl = new Role();
                    n_rl.name = rl.Value;
                    n_rl.namepic = rl.TagPicture;
                    n_rl.role = rl.Role;
                    n_rl.roledesc = rl.RoleDescription;
                    n_rl.rolepic = rl.RolePicture;
                    roles.Add(n_rl);
                }
            }

            if (aep.PlexContract?.Tags != null)
            {
                foreach (JMMContracts.PlexAndKodi.Tag tg in aep.PlexContract?.Tags)
                {
                    Tag n_tg = new Tag();
                    n_tg.tag = tg.Value;
                    tags.Add(n_tg);
                }
            }
            
            // until fanart refactor this will be good for start
            art.thumb.Add(new Art() { url = aep.PlexContract?.Thumb, index = 0 });
            art.fanart.Add(new Art() { url = aep.PlexContract?.Art, index = 0 });

            List<VideoLocal> vls = aep.GetVideoLocals();
            if (vls.Count > 0)
            {
                files = new List<RawFile>();
                foreach (VideoLocal vl in vls)
                {
                    files.Add(new RawFile(vl));
                }
            }
        }
    }
}