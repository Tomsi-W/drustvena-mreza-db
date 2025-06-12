using Microsoft.AspNetCore.Mvc;
using Drustvena_mreza_Clanovi_i_grupe.Repositories;
using Drustvena_mreza_Clanovi_i_grupe.Models;

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
			var result = repository.GetAllPostsWithUsernames();
			return Ok(result);
		}
	}
}
