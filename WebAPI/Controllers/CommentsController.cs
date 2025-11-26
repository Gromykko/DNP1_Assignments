using System.Collections;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository CommentRepo;
        private readonly IUserRepository userRepository;

        public CommentsController(ICommentRepository CommentRepo, IUserRepository userRepository)
        {
            this.CommentRepo = CommentRepo;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> AddComment([FromBody] CreateCommentDto request)
        {
            Comment Comment = new(request.PostId, request.UserId, request.Body);
            Comment created = await CommentRepo.AddAsync(Comment);

            return Created($"/Comments/{created.Id}", created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Comment>> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
        {
            Comment CommentToUpdate = await CommentRepo.GetSingleAsync(id);
            CommentToUpdate.Body = request.Body;
            await CommentRepo.UpdateAsync(CommentToUpdate);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Comment>> GetComment([FromRoute] int id)
        {
            Comment CommentToGet = await CommentRepo.GetSingleAsync(id);
            return Ok(CommentToGet);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments([FromQuery] int? writtenById = null, [FromQuery] string? writtenByName = null, [FromQuery] int? postId = null)
        {
            IQueryable<Comment> comments = CommentRepo.GetMany();

            if (writtenById != null)
            {
                comments = comments.Where(c => c.UserId == writtenById);
            }

            if (writtenByName != null)
            {
                var userIds = userRepository.GetMany()
                    .Where(u => u.Username.Equals(writtenByName))
                    .Select(u => u.Id);
                comments = comments.Where(c => userIds.Contains(c.UserId));
            }

            if (postId != null)
            {
                comments = comments.Where(c => c.PostId == postId);
            }

            return Ok(await comments.ToListAsync());
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Comment>> DeleteComment([FromRoute] int id)
        {
            await CommentRepo.DeleteAsync(id);
            return NoContent();
        }
    }


}
