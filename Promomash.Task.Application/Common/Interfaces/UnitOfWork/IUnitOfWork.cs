namespace PromomashTask.Application.Common.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
	Task CommitChangesAsync(CancellationToken cancellationToken = default);
}
