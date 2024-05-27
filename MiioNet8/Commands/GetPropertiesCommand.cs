using MiioNet8.Interfaces;

namespace MiioNet8.Commands
{
    internal class GetPropertiesCommand : BaseCommand
    {
        public GetPropertiesCommand(IEnumerable<ISpecServiceProperty> properties) : base("get_properties") =>
            Params.AddRange(properties.Select(p =>
                new Property($"{p.Service.Description.ToLower()}:{p.Description.ToLower()}", p.Service.Id, p.Id)));

        public GetPropertiesCommand(IEnumerable<(string did, int sid, int pid)> properties) : base("get_properties") =>
            Params.AddRange(properties.Select(p => new Property(p.did, p.sid, p.pid)));
    }
}