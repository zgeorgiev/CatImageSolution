using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    /// <summary>
    /// Result is a class used to 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        public Result()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        public Result(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// The success.
        /// </summary>
        public bool Success => !Errors.Any();

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}
