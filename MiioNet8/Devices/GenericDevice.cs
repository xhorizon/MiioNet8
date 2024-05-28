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

        internal async Task<(CommunicationResult, List<JsonElement>?)> GetRawPropertiesAsync(GetPropertiesCommand command)
        {
            var (result, response) = await SendCommandAsync<GetRawPropertiesResponse>(command);

            if (result != CommunicationResult.Success)
                return (result, null);

            return (result, response?.Result);
        }

        internal async Task<(CommunicationResult, List<Property>?)> GetPropertiesAsync(GetPropertiesCommand command)
        {
            var (result, response) = await SendCommandAsync<GetPropertiesResponse>(command);

            if (result != CommunicationResult.Success)
                return (result, null);

            return (result, response?.Result);
        }

        internal async Task<string?> GetStringPropertyAsync(GetPropertiesCommand command)
        {
            var (communicationResult, result) = await GetPropertiesAsync(command);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");

            if (result[0].Value is JsonElement jsonElement)
            {
                var value = jsonElement.Deserialize<string>();

                return value;
            }

            throw new Exception();
        }

        internal async Task<T> GetPropertyAsync<T>(GetPropertiesCommand command) where T : struct
        {
            var (communicationResult, result) = await GetPropertiesAsync(command);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");

            if (result[0].Value is JsonElement jsonElement)
            {
                var value = jsonElement.Deserialize<T>();

                return value;
            }

            throw new Exception();
        }


        internal async Task<List<object>> GetPropertyRawAsync(GetPropertiesCommand command)
        {
            var (communicationResult, result) = await GetPropertiesAsync(command);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");


            throw new Exception();
        }


        protected async Task<(CommunicationResult, List<Property>?)> GetPropertiesAsync(
            List<ISpecServiceProperty> properties)
        {
            return await GetPropertiesAsync(new GetPropertiesCommand(properties));
        }

        protected async Task<T> GetPropertyAsync<T>(ISpecServiceProperty property) where T : struct
        {
            var (communicationResult, result) = await GetPropertiesAsync([property]);

            if (communicationResult != CommunicationResult.Success || result?.Count != 1)
                throw new Exception("");

            if (result[0].Value is JsonElement jsonElement)
            {
                var value = jsonElement.Deserialize<T>();

                return value;
            }

            throw new Exception();
        }

        protected async Task<T> GetPropertyAsync<T>(string serviceName, string propertyName) where T : struct
        {
            if (!GetSpecServiceProperty(serviceName, propertyName, out var property))
                throw new Exception("");

            return await GetPropertyAsync<T>(property!);
        }

        protected async Task SetPropertiesAsync(List<(ISpecServiceProperty, object)> propertiesWithValue)
        {
            var (result, response) = await SendCommandAsync<BaseResponse>(
                new SetPropertiesCommand(propertiesWithValue)
            );
        }

        protected async Task SetPropertyAsync(ISpecServiceProperty property, object value)
        {
            await SetPropertiesAsync([(property, value)]);
        }

        protected async Task SetPropertyAsync(string serviceName, string propertyName, object value)
        {
            if (!GetSpecServiceProperty(serviceName, propertyName, out var property))
                throw new Exception("");

            await SetPropertyAsync(property!, value);
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