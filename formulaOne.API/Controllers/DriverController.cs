using AutoMapper;
using formulaOne.API.DTOs.Requests.Driver;
using formulaOne.API.DTOs.Responses;
using formulaOne.API.Extensions;
using formulaOne.DataService.Repositories.Interfaces;
using formulaOne.Entities.DbSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace formulaOne.API.Controllers;

[Route("/api/driver")]
public class DriverController : BaseController
{
    public DriverController(IUnitOfWork unitOfWork, IMapper mapper, Serilog.ILogger logger, IDistributedCache cache) : base(unitOfWork, mapper, logger, cache) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DriverResponseDto>> CreateDrverAsync([FromBody] CreateDriverDto createDriverDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            Driver driver = _mapper.Map<Driver>(createDriverDto);

            driver = await _unitOfWork.driverRepository.AddAsync(driver);

            await _unitOfWork.SaveChangesAsync();

            DriverResponseDto driverResponseDto = _mapper.Map<DriverResponseDto>(driver);

            return CreatedAtAction(nameof(GetDriverAsync), new { id = driverResponseDto.Id }, driverResponseDto);
        }
        catch (ApplicationException)
        {
            _logger.Error("Error occurred while creating a new driver");

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create a new driver");
        }
    }

    [HttpGet("{id:guid}", Name = "GetDriverAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DriverResponseDto>> GetDriverAsync(Guid id)
    {
        try
        {
            Driver? driver = await _unitOfWork.driverRepository.GetByIdAsync(id);

            if (driver is null) return NotFound();

            DriverResponseDto driverResponseDto = _mapper.Map<DriverResponseDto>(driver);

            return Ok(driverResponseDto);
        }
        catch (ApplicationException)
        {
            _logger.Error("Error occurred while retrieving a driver");

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve a driver");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<DriverResponseDto>>> GetDriversAsync()
    {
        try
        {
            string recordKey = $"DriverController_GetDriversAsync_{DateTime.UtcNow.ToString("yyyyMMdd_hhmm")}";

            IEnumerable<Driver>? drivers = await _distributedCache.GetRecordAsync<IEnumerable<Driver>>(recordKey);

            if (drivers is null)
            {
                drivers = await _unitOfWork.driverRepository.GetAllAsync();

                if (drivers.Any())
                {
                    await _distributedCache.SetRecordAsync<IEnumerable<Driver>>(recordKey, drivers);
                }
            }

            IEnumerable<DriverResponseDto> driverResponseDtos = _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);

            return Ok(driverResponseDtos);
        }
        catch (ApplicationException)
        {
            _logger.Error("Error occurred while retrieving drivers");

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve drivers");
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDriverAsync(Guid id, [FromBody] UpdateDriverDto updateDriverDto)
    {
        if (id != updateDriverDto.Id) return BadRequest("Invalid id");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            Driver? driver = _mapper.Map<Driver>(updateDriverDto);

            driver = await _unitOfWork.driverRepository.UpdateAsync(id, driver);

            if (driver is null) return NotFound($"Driver with id {id} could not be found");

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (ApplicationException)
        {
            _logger.Error("Error occurred while updating a driver");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update driver");
        }
    }

}