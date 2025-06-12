using Microsoft.AspNetCore.Mvc;
using Drustvena_mreza_Clanovi_i_grupe.Repositories;
using Drustvena_mreza_Clanovi_i_grupe.Models;
using System.Collections.Generic;

namespace Drustvena_mreza_Clanovi_i_grupe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostDbRepository repository;

        public PostController(PostDbRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAllPosts()
        {
            List<Post> result = repository.GetAllPostsWithUsernames();
            return Ok(result);
        }
    }
}
