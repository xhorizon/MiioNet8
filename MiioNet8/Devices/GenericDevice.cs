using MiioNet8.Commands;
using MiioNet8.Communication;
using MiioNet8.Interfaces;
using MiioNet8.Miot;
using MiioNet8.Protocol;
using MiioNet8.Responses;
using System.Net;
using System.Text.Json;

namespace MiioNet8.Devices
{
    public class GenericDevice : BaseDevice
    {
        public GenericDevice(ICommunication communication, IToken token) : base(communication, token)
        {
        }

        internal async Task<(CommunicationResult, List<JsonElement>?)> GetRawPropertiesAsync(
            GetPropertiesCommand command, CancellationToken token = default)
        {
            var (result, response) = await SendCommandAsync<GetRawPropertiesResponse>(command, token);

            if (result != CommunicationResult.Success)
                return (result, null);

            return (result, response?.Result);
        }

        internal async Task<(CommunicationResult, List<Property>?)> GetPropertiesAsync(GetPropertiesCommand command,
            CancellationToken token = default)
        {
            var (result, response) = await SendCommandAsync<GetPropertiesResponse>(command, token);

            if (result != CommunicationResult.Success)
                return (result, null);

            return (result, response?.Result);
        }

        internal async Task<string?> GetStringPropertyAsync(GetPropertiesCommand command,
            CancellationToken token = default)
        {
            var (communicationResult, result) = await GetPropertiesAsync(command, token);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");

            if (result[0].Value is JsonElement jsonElement)
            {
                var value = jsonElement.Deserialize<string>();

                return value;
            }

            throw new Exception();
        }

        internal async Task<T> GetPropertyAsync<T>(GetPropertiesCommand command, CancellationToken token = default)
            where T : struct
        {
            var (communicationResult, result) = await GetPropertiesAsync(command, token);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");

            if (result[0].Value is JsonElement jsonElement)
            {
                var value = jsonElement.Deserialize<T>();

                return value;
            }

            throw new Exception();
        }


        internal async Task<List<object>> GetPropertyRawAsync(GetPropertiesCommand command,
            CancellationToken token = default)
        {
            var (communicationResult, result) = await GetPropertiesAsync(command, token);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");


            throw new Exception();
        }


        protected async Task<(CommunicationResult, List<Property>?)> GetPropertiesAsync(
            List<ISpecServiceProperty> properties, CancellationToken token = default)
        {
            return await GetPropertiesAsync(new GetPropertiesCommand(properties), token);
        }

        protected async Task<T> GetPropertyAsync<T>(ISpecServiceProperty property, CancellationToken token = default)
            where T : struct
        {
            var (communicationResult, result) = await GetPropertiesAsync([property], token);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");

            if (result[0].Value is JsonElement jsonElement)
            {
                var value = jsonElement.Deserialize<T>();

                return value;
            }

            throw new Exception();
        }

        protected async Task<T> GetPropertyAsync<T>(string serviceName, string propertyName,
            CancellationToken token = default)
            where T : struct
        {
            if (!GetSpecServiceProperty(serviceName, propertyName, out var property))
                throw new Exception("");

            return await GetPropertyAsync<T>(property!, token);
        }

        protected async Task SetPropertiesAsync(List<(ISpecServiceProperty, object)> propertiesWithValue,
            CancellationToken token = default)
        {
            var (result, response) = await SendCommandAsync<BaseResponse>(
                new SetPropertiesCommand(propertiesWithValue),
                token
            );
        }

        protected async Task SetPropertyAsync(ISpecServiceProperty property, object value,
            CancellationToken token = default)
        {
            await SetPropertiesAsync([(property, value)], token);
        }

        protected async Task SetPropertyAsync(string serviceName, string propertyName, object value,
            CancellationToken token = default)
        {
            if (!GetSpecServiceProperty(serviceName, propertyName, out var property))
                throw new Exception("");

            await SetPropertyAsync(property!, value, token);
        }

        protected bool GetSpecServiceProperty(string serviceName, string propertyName,
            out ISpecServiceProperty? property)
        {
            property = Spec.Properties.FirstOrDefault(p =>
                p.Service.Description.ToLower() == serviceName.ToLower() &&
                p.Description.ToLower() == propertyName.ToLower());

            return property != null;
        }
    }
}