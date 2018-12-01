using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    // TODO 5: unauthorized users should receive 401 status code and should be redirected to Login endpoint --> KIND OF DONE (see Startup.cs)   
    [Route("api/account")]
    public class AccountController : Controller
    {
        
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        public ValueTask<Account> Get()
        {
            return _accountService.LoadOrCreateAsync(User.Identity.Name/* TODO 3: Get user id from cookie --> DONE */);
        }
        
        //TODO 6: Get user id from cookie --> ??? Why? It ruins the point of the method
        //TODO 7: Endpoint should works only for users with "Admin" Role --> DONE (also see LoginController)
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
        {
            return _accountService.GetFromCache(id);
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            //Update account in cache, don't bother saving to DB, this is not an objective of this task.
            var account = await Get();
            account.Counter++;
        }
    }
}