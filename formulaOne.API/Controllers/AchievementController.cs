using AutoMapper;
using formulaOne.API.DTOs.Requests;
using formulaOne.API.DTOs.Requests.Achievement;
using formulaOne.API.DTOs.Responses;
using formulaOne.API.Extensions;
using formulaOne.DataService.Repositories.Interfaces;
using formulaOne.Entities.DbSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace formulaOne.API.Controllers;

[Route("/api/achievement")]
public class AchievementController : BaseController
{
    public AchievementController(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger, IDistributedCache cache) : base(unitOfWork, mapper, logger, cache)
    { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AchievementResponseDto>> CreateAchievementAsync([FromBody] CreateAchievementDto createAchievementDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Achievement achievement = _mapper.Map<Achievement>(createAchievementDto);

            achievement = await _unitOfWork.achievementRepository.AddAsync(achievement);

            await _unitOfWork.SaveChangesAsync();

            AchievementResponseDto achievementResponseDto = _mapper.Map<AchievementResponseDto>(achievement);

            return CreatedAtAction(nameof(GetAchievementAsync), new { id = achievementResponseDto.Id }, achievementResponseDto);
        }
        catch (ApplicationException)
        {
            _logger.LogError("Error occurred while trying to add a new achievement to the database");

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create a new achievement");
        }
    }

    [HttpGet("{id:guid}", Name = "GetAchievementAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AchievementResponseDto>> GetAchievementAsync(Guid id)
    {
        try
        {

            Achievement? achievement = await _unitOfWork.achievementRepository.GetByIdAsync(id);

            if (achievement is null) return NotFound($"Achievement with id {id} does not exist");

            AchievementResponseDto achievementResponseDto = _mapper.Map<AchievementResponseDto>(achievement);

            return Ok(achievementResponseDto);
        }
        catch (ApplicationException)
        {
            _logger.LogError("Error occurred while trying to retrieve an achievement from the database");
            return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to retrieve achievement with id {id}");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AchievementResponseDto>>> GetAchievementsAsync()
    {
        try
        {
            string recordKey = $"AchievementController_GetAchievementsAsync_{DateTime.UtcNow.ToString("yyyyMMdd_hhmm")}";

            IEnumerable<Achievement>? achievements = await _distributedCache.GetRecordAsync<IEnumerable<Achievement>>(recordKey);

            if (achievements is null)
            {
                achievements = await _unitOfWork.achievementRepository.GetAllAsync();

                if (achievements.Any())
                {
                    await _distributedCache.SetRecordAsync<IEnumerable<Achievement>>(recordKey, achievements);
                }
            }

            IEnumerable<AchievementResponseDto> achievementResponseDtos = _mapper.Map<IEnumerable<AchievementResponseDto>>(achievements);

            return Ok(achievementResponseDtos);
        }
        catch (ApplicationException)
        {
            _logger.LogError("Error occurred while retrieving all achievements");

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve achievements");
        }
    }

    [HttpGet("{driverId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AchievementResponseDto>>> GetDriverAchievementsAsync(Guid driverId)
    {
        try
        {
            string recordKey = $"AchievementController_GetDriverAchievementsAsync_{DateTime.UtcNow.ToString("yyyyMMdd_hhmm")}";

            IEnumerable<Achievement>? achievements = await _distributedCache.GetRecordAsync<IEnumerable<Achievement>>(recordKey);

            if (achievements is null)
            {
                achievements = await _unitOfWork.achievementRepository.GetDriversAchievementsByIdAsync(driverId);

                if (achievements.Any())
                {
                    await _distributedCache.SetRecordAsync<IEnumerable<Achievement>>(recordKey, achievements);
                }
            }

            IEnumerable<AchievementResponseDto> achievementResponseDtos = _mapper.Map<IEnumerable<AchievementResponseDto>>(achievements);

            return Ok(achievementResponseDtos);
        }
        catch (ApplicationException)
        {
            _logger.LogError("Error occurred while retrieving achievements for a driver");

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve driver's achievements");
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAchievementAsync(Guid id, [FromBody] UpdateAchievementDto updateAchievementDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != updateAchievementDto.Id) return BadRequest("Invalid id");

        try
        {
            Achievement? achievement = _mapper.Map<Achievement>(updateAchievementDto);

            achievement = await _unitOfWork.achievementRepository.UpdateAsync(id, achievement);

            if (achievement is null) return NotFound();

            return NoContent();
        }
        catch
        {
            _logger.LogError("Error occurred while updating achievement");

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update an achievement");
        }
    }


}