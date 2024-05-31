using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<ReturnAnalyticsDTO>>>GetAnalytics(AnalyticsDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var analytics = await _analyticsService.GetAnalytics(dto);
                return Ok(analytics);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        [Route("overdue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<ReturnODAnalyticsDTO>>> GetOverdueAnalytics(AnalyticsDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var analytics = await _analyticsService.returnODAnalyticsDTOs(dto);
                return Ok(analytics);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
