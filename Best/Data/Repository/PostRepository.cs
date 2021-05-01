﻿using Best.Data.Interfaces;
using Best.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class PostRepository : IPosts
    {
        private readonly BestContent bestContent;
        public PostRepository(BestContent bestContent)
        {
            this.bestContent = bestContent;
        }
        public IEnumerable<Post> GetPosts => bestContent.Post.Include(c => c.Campaing).Include(u => u.BestUser);
        public Post GetPostById(string post_id) => GetPosts.FirstOrDefault(p => p.Id == post_id);
        public IEnumerable<Post> GetPostsByCampaingId(string campaing_Id) => GetPosts.Where(p => p.Campaing.Id == campaing_Id);
        public IEnumerable<Post> GetPostsByUserId(string user_Id) => GetPosts.Where(p => p.Campaing.BestUser.Id == user_Id);
        public Post GetPostByIdForUser(string user_id, string post_id) => GetPostsByUserId(user_id).FirstOrDefault(p => p.Id == post_id);
        //CRUD
        public async Task<int> Create(Post post)
        {
            bestContent.Post.Add(post);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> Update(Post post)
        {
            bestContent.Post.Update(post);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> Delete(Post post)
        {
            bestContent.Post.Remove(post);
            return await bestContent.SaveChangesAsync();
        }
        public async Task<int> DeletePostsByCampaingId(string campaing_Id)
        {
            var posts = GetPostsByCampaingId(campaing_Id).ToList();
            int result = 0;
            foreach (var post in posts)
            {
                result += await Delete(post);
            }
            return result;
        }
    }
}
