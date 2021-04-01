#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Menhera.Classes.Files;
using Menhera.Database;
using Menhera.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Menhera.Classes.Db
{
    public static class DbAccess
    {
        //Тред не может быть создан без поста, поэтому передаем сам новый пост,
        //а внутри метода создаём новый тред, в который добавляем этот пост
        public static void AddThreadToBoard(MenherachanContext db, ref Post firstThreadPost)
        {
            var thread = new Thread
            {
                AnonName = firstThreadPost.AnonName,
                BoardId = firstThreadPost.BoardId,
                OpIpHash = firstThreadPost.AnonIpHash,
                IsClosed = false
            };

            db.Thread.Add(thread);
            db.SaveChanges();

            firstThreadPost.ThreadId = thread.ThreadId;
            firstThreadPost.TimeInUnixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

            db.Post.Add(firstThreadPost);
            db.SaveChanges();
        }

        public static void AddPostToThread(MenherachanContext db, Post post)
        {
            var thread = db.Thread.First(t => t.ThreadId == post.ThreadId);

            if (thread == null)
            {
                throw new InvalidOperationException();
            }

            db.Post.Add(post);
            db.SaveChanges();
        }

        public static void AddFilesToPost(MenherachanContext db, Post post, ImageInformation info)
        {
            db.File.Add(new File
            {
                BoardId = post.BoardId,
                ThreadId = post.ThreadId,
                PostId = post.PostId,
                FileName = info.FileName,
                ThumbnailName = info.ThumbnailName,
                Info = $"{info.Information}"
            });
            db.SaveChanges();
        }
    }
}