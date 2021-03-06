using Microsoft.WindowsAzure.MediaServices.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace vodworkflow
{
    public class EncodeJob
    {
        public string assetId { get; set; }
        public string mesPreset { get; set; }
        public EncodeJob(string AssetId, string MesPreset)
        {
            assetId = AssetId;
            mesPreset = MesPreset;
        }
    }

    public class mes
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public mes(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class mepw
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public mepw(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class indexV1
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public string language { get; set; }
        public indexV1(string AssetId, string TaskId, string Language)
        {
            assetId = AssetId;
            taskId = TaskId;
            language = Language;
        }
    }

    public class indexV2
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public string language { get; set; }
        public indexV2(string AssetId, string TaskId, string Language)
        {
            assetId = AssetId;
            taskId = TaskId;
            language = Language;
        }
    }

    public class ocr
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public ocr(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class faceDetection
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public faceDetection(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class faceRedaction
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public faceRedaction(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class motionDetection
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public motionDetection(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class summarization
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public summarization(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class hyperlapse
    {
        public string assetId { get; set; }
        public string taskId { get; set; }
        public hyperlapse(string AssetId, string TaskId)
        {
            assetId = AssetId;
            taskId = TaskId;
        }
    }

    public class EncodeJobResponse
    {
        public string id { get; set; }
        public string jobId { get; set; }
        public string otherJobsQueue { get; set; }
        public mes Mes { get; set; }
        public indexV1 IndexV1 { get; set; }
        public indexV2 IndexV2 { get; set; }
        public ocr Ocr { get; set; }
        public faceDetection FaceDetection { get; set; }
        public faceRedaction FaceRedaction { get; set; }
        public motionDetection MotionDetection { get; set; }
        public summarization Summarization { get; set; }
        public hyperlapse Hyperlapse { get; set; }

        public EncodeJobResponse() { }
    }

    public class CheckJob
    {
        string jobId { get; set; }
        string extendedInfo { get; set; }
        public CheckJob(string JobId, string ExtendedInfo)
        {
            jobId = JobId;
            extendedInfo = ExtendedInfo;
        }
    }

    public class extendedInfo
    {
        public string mediaUnitNumber { get; set; }
        public string mediaUnitSize { get; set; }
        public string otherJobsProcessing { get; set; }
        public string otherJobsScheduled { get; set; }
        public string otherJobsQueue { get; set; }
        public string amsRESTAPIEndpoint { get; set; }
    }

    public class CheckJobResponse
    {
        public string jobState { get; set; }       // The state of the job (int)
        public string isRunning { get; set; }      // True if job is running
        public string isSuccessful { get; set; }   // True is job is a success. Only valid if IsRunning = False
        public string errorText { get; set; }      // error(s) text if job state is error
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string runningDuration { get; set; }
        public extendedInfo extinfo { get; set; }
    }

    class Program
    {
        // Read values from the App.config file.
        static string _AADTenantDomain = ConfigurationManager.AppSettings["AMSAADTenantDomain"];
        static string _RESTAPIEndpoint = ConfigurationManager.AppSettings["AMSRESTAPIEndpoint"];
        static string _ClientID = ConfigurationManager.AppSettings["clientid"];
        static string _ClientSecret = ConfigurationManager.AppSettings["clientsecret"];

        private static readonly string _mediaFiles = "C:\\dev\\go\\src\\thumbnails\\video-nginx\\html\\video\\bigbuck.mp4";


        // Field for service context.
        private static CloudMediaContext _context = null;

        static void Main(string[] args)
        {
            try
            {
                AzureAdTokenCredentials tokenCredentials = new AzureAdTokenCredentials(_AADTenantDomain, new AzureAdClientSymmetricKey(_ClientID, _ClientSecret),
                                                                AzureEnvironments.AzureCloudEnvironment);

                AzureAdTokenProvider tokenProvider = new AzureAdTokenProvider(tokenCredentials);

                _context = new CloudMediaContext(new Uri(_RESTAPIEndpoint), tokenProvider);

                IMediaProcessor processor = GetLatestMediaProcessorByName("Media Encoder Standard");
                

                // If you want to secure your high quality input media files with strong encryption at rest on disk,
                // use AssetCreationOptions.StorageEncrypted instead of AssetCreationOptions.None.

                Console.WriteLine("Upload a file.\n");
                //IAsset inputAsset = UploadFile(_mediaFiles, AssetCreationOptions.None);

                // create adaptive bitrate set
                string assetId = "nb:cid:UUID:59e2bc1d-1726-4645-bece-1b98faca5c8d";
                EncodeJobResponse encJob = EncodeToAdaptiveBitrateMP4Set(assetId);

                // create associated .vtt file

                // uplaod text file to asset name

                // check status of encode
                CheckJobResponse chkJob;
                do
                {
                    chkJob = CheckJobStatus(encJob.jobId);
                } while (chkJob.isRunning.Equals("true"));
       
            }
            catch (Exception exception)
            {
                // Parse the XML error message in the Media Services response and create a new
                // exception with its content.
                exception = MediaServicesExceptionParser.Parse(exception);

                Console.Error.WriteLine(exception.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static public IAsset UploadFile(string fileName, AssetCreationOptions options)
        {
            Console.WriteLine("Upload File: filename, options", fileName, OperationState.Succeeded);

            //_context.Assets.Create("qelloinput", "qellofirst", options);
            IAsset inputAsset = _context.Assets.CreateFromFile(
                fileName,
                options,
                (af, p) =>
                {
                    Console.WriteLine("Uploading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                });

            Console.WriteLine("Asset {0} created.", inputAsset.Id);

            return inputAsset;
        }

        // Send a message to encode to MP4
        static public EncodeJobResponse EncodeToAdaptiveBitrateMP4Set(string assetId)
        {
            // send a message to submit job 
            // string url = "https://azmediafunctionsforlogicappwdahb73ofbb5k.azurewebsites.net/api/submit-job?code=J3mX1K4aWOMC6PXiTDkHY/BL1sxxgQy2IBJs0L9Vhs6Z158ucNjNpA==&clientId=default";
            string url = "http://192.168.1.16:3000/submitjob/1";

            EncodeJob ejob = new EncodeJob(assetId, "Content Adaptive Multiple Bitrate MP4");
            string json = JsonConvert.SerializeObject(ejob);
            HttpWebResponse response = MakeHttpRequest(url, json, "POST");

            EncodeJobResponse encJob = ReadEncodeJobResponse(response);

            return encJob;
        }
        
        static public void AddSubtitletoAsset(string srtFile, IAsset asset)
        {

        }

        private static IMediaProcessor GetLatestMediaProcessorByName(string mediaProcessorName)
        {
            var processor = _context.MediaProcessors.Where(p => p.Name == mediaProcessorName).
            ToList().OrderBy(p => new Version(p.Version)).LastOrDefault();

            if (processor == null)
                throw new ArgumentException(string.Format("Unknown media processor", mediaProcessorName));

            return processor;
        }

        public static CheckJobResponse CheckJobStatus(string jobid)
        {
            CheckJob job = new CheckJob(jobid, "true");
            string url = "http://192.168.1.16:3000/submitjob/1";

            string json = JsonConvert.SerializeObject(job);
            HttpWebResponse response = MakeHttpRequest(url, json, "POST");
            CheckJobResponse chkJob = ReadCheckJobResponse(response);

            return chkJob;

        }

        public static HttpWebResponse MakeHttpRequest(string uri, string json, string verb)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(json);
            HttpWebRequest objHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

            if (verb.Equals("POST"))
            {
                objHttpWebRequest.Method = "POST";
                objHttpWebRequest.ContentType = "application/json";
                objHttpWebRequest.KeepAlive = true;
                objHttpWebRequest.ContentLength = bytes.Length;
                Stream objStream = objHttpWebRequest.GetRequestStream();
                objStream.Write(bytes, 0, bytes.Length);
                objStream.Close();
            }
            else
            {
                objHttpWebRequest.Method = "GET";

            }


            try
            { 
                HttpWebResponse objHttpWebResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();
                return objHttpWebResponse;
            }
            catch(WebException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }

            return null;
        }

        public static EncodeJobResponse ReadEncodeJobResponse(HttpWebResponse httpResponse)
        {
            EncodeJobResponse encJob = new EncodeJobResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string result = reader.ReadToEnd();
                            encJob = JsonConvert.DeserializeObject<EncodeJobResponse>(result);
                        }
                    }
                }
                catch (WebException exception)
                {
                    Console.Error.WriteLine(exception.Message);
                }
            }
  
            return encJob;
        }

        public static CheckJobResponse ReadCheckJobResponse(HttpWebResponse httpResponse)
        {
            Stream stream = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            CheckJobResponse chkJob = JsonConvert.DeserializeObject<CheckJobResponse>(result);
            return chkJob;
        }

        public static void TestJson()
        {
            string result = @"{
    'jobId': 'nb:jid:UUID:19cab2ff-0300-80c0-da01-f1e7d467f6f9',
    'otherJobsQueue': 1,
    'mes': {
                    'assetId': 'nb:cid:UUID:7346ded3-fdb5-4e97-b508-4e20b054d0b4',
        'taskId': 'nb:tid:UUID:19cab2ff-0300-80c0-da02-f1e7d467f6f9'
    },
    'mepw': {
                    'assetId': null,
        'taskId': null
    },
    'indexV1': {
                    'assetId': null,
        'taskId': null,
        'language': null
    },
    'indexV2': {
                    'assetId': 'nb:cid:UUID:7205d19f-21da-4cb1-9dbb-47d251103d75',
        'taskId': 'nb:tid:UUID:19cab2ff-0300-80c0-da03-f1e7d467f6f9',
        'language': 'EnUs'
    },
    'ocr': {
                    'assetId': null,
        'taskId': null
    },
    'faceDetection': {
                    'assetId': null,
        'taskId': null
    },
    'faceRedaction': {
                    'assetId': null,
        'taskId': null
    },
    'motionDetection': {
                    'assetId': null,
        'taskId': null
    },
    'summarization': {
                    'assetId': null,
        'taskId': null
    },
    'hyperlapse': {
                    'assetId': null,
        'taskId': null
    }
            }";

            EncodeJobResponse encJob = JsonConvert.DeserializeObject<EncodeJobResponse>(result);
        }
    }


}
