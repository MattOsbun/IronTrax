using Irontrax.Api.Activity.Models;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Irontrax.Api.Activity.Data;
using StackExchange.Redis;

namespace Irontrax.Api.Activity
{
    public class ExerciseApi
    {
        private static ConnectionMultiplexer connectionMultiplexer;

        public ExerciseApi()
        {

        }

        internal ExerciseApi(string redisConnectionString)
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise> CreateExercise(Exercise exercise)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise> GetExerciseById(string exerciseId)
        {
            throw new NotImplementedException();
        }

        public static async Task CreateNewExercise()
        {
            throw new NotImplementedException();
        }
        public static async Task UpdateExerciseDescription()
        {
            throw new NotImplementedException();
        }

        public static async Task DeleteExercise(string id)
        {
            throw new NotImplementedException();
        }
    }
}
