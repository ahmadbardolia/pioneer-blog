﻿#if (DEBUG)
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pioneer.Blog.Model;
using Pioneer.Blog.Service;

namespace Pioneer.Blog.Areas.Admin.Controllers.Api
{
    [Authorize]
    [Route("api/posts")]
    public class PostApiController : Controller
    {
        private readonly IPostService _postService;

        public PostApiController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">Total nodes in page</param>
        /// <param name="page">What page are we on</param>
        /// <param name="includeExceprt"></param>
        /// <param name="includeArticle"></param>
        /// <param name="includeUnpublished"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Post> GetAll(int? count, 
            int? page, 
            bool includeExceprt = true, 
            bool includeArticle = true, 
            bool includeUnpublished = false)
        {
            if (count == null || page == null)
            {
                return _postService.GetAll(includeExceprt, includeArticle, includeUnpublished);
            }

            return _postService.GetAllPaged((int)count, (int)page);
        }

        [HttpGet("{url}", Name = "GetPost")]
        public IActionResult GetById(string url, bool includeExcerpt = false)
        {
            var item = _postService.GetByUrl(url, includeExcerpt);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        [Authorize(Policy = "isSuperUser")]
        public IActionResult Create([FromBody]Post post)
        {
            if (post == null)
            {
                return BadRequest();
            }

            _postService.Add(post);
            return CreatedAtRoute("GetPost", new { url = post.Url }, post);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "isSuperUser")]
        public IActionResult Update(int id, [FromBody] Post item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var todo = _postService.GetById(id);
            if (todo == null)
            {
                return NotFound();
            }

            _postService.Update(item);
            return new NoContentResult();
        }

        //[HttpDelete("{url}")]
        //[Authorize(Policy = "isSuperUser")]
        //public IActionResult Delete(string url)
        //{
        //    var todo = _postService.GetByUrl(url);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    _postService.Remove(url);
        //    return new NoContentResult();
        //}
    }
}
#endif