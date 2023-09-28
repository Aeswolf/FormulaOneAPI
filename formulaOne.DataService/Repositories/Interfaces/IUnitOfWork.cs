namespace formulaOne.DataService.Repositories.Interfaces;

public interface IUnitOfWork
{
    IDriverRepository driverRepository { get; }
    IAchievementRepository achievementRepository { get; }

    Task SaveChangesAsync();
}