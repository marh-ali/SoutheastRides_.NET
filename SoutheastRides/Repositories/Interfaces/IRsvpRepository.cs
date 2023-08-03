using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRsvpRepository
{
    Task<IEnumerable<Rsvp>> GetAll();
    Task<Rsvp> Get(string id);
    Task<Rsvp> Create(Rsvp rsvp);
    Task Update(string id, Rsvp rsvp);
    Task Delete(string id);
}
