/**
 * @fileoverview gRPC-Web generated client stub for 
 * @enhanceable
 * @public
 */

// GENERATED CODE -- DO NOT EDIT!


/* eslint-disable */
// @ts-nocheck



const grpc = {};
grpc.web = require('grpc-web');

const proto = require('./issueTracker_pb.js');

/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?grpc.web.ClientOptions} options
 * @constructor
 * @struct
 * @final
 */
proto.IssueTrackerServiceClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options.format = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

};


/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?grpc.web.ClientOptions} options
 * @constructor
 * @struct
 * @final
 */
proto.IssueTrackerServicePromiseClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options.format = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.AddIssueMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerService_AddIssue = new grpc.web.MethodDescriptor(
  '/IssueTrackerService/AddIssue',
  grpc.web.MethodType.UNARY,
  proto.AddIssueMessage,
  proto.StatusMessage,
  /**
   * @param {!proto.AddIssueMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.AddIssueMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerServiceClient.prototype.addIssue =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerService/AddIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_AddIssue,
      callback);
};


/**
 * @param {!proto.AddIssueMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerServicePromiseClient.prototype.addIssue =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerService/AddIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_AddIssue);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.EditIssueMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerService_EditIssue = new grpc.web.MethodDescriptor(
  '/IssueTrackerService/EditIssue',
  grpc.web.MethodType.UNARY,
  proto.EditIssueMessage,
  proto.StatusMessage,
  /**
   * @param {!proto.EditIssueMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.EditIssueMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerServiceClient.prototype.editIssue =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerService/EditIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_EditIssue,
      callback);
};


/**
 * @param {!proto.EditIssueMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerServicePromiseClient.prototype.editIssue =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerService/EditIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_EditIssue);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueByIdQueryMessage,
 *   !proto.IssueMessage>}
 */
const methodDescriptor_IssueTrackerService_GetIssueById = new grpc.web.MethodDescriptor(
  '/IssueTrackerService/GetIssueById',
  grpc.web.MethodType.UNARY,
  proto.IssueByIdQueryMessage,
  proto.IssueMessage,
  /**
   * @param {!proto.IssueByIdQueryMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.IssueMessage.deserializeBinary
);


/**
 * @param {!proto.IssueByIdQueryMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.IssueMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.IssueMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerServiceClient.prototype.getIssueById =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerService/GetIssueById',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_GetIssueById,
      callback);
};


/**
 * @param {!proto.IssueByIdQueryMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.IssueMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerServicePromiseClient.prototype.getIssueById =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerService/GetIssueById',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_GetIssueById);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.PagedIssueRequestMessage,
 *   !proto.IssueSummariesMessage>}
 */
const methodDescriptor_IssueTrackerService_GetIssues = new grpc.web.MethodDescriptor(
  '/IssueTrackerService/GetIssues',
  grpc.web.MethodType.UNARY,
  proto.PagedIssueRequestMessage,
  proto.IssueSummariesMessage,
  /**
   * @param {!proto.PagedIssueRequestMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.IssueSummariesMessage.deserializeBinary
);


/**
 * @param {!proto.PagedIssueRequestMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.IssueSummariesMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.IssueSummariesMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerServiceClient.prototype.getIssues =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerService/GetIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_GetIssues,
      callback);
};


/**
 * @param {!proto.PagedIssueRequestMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.IssueSummariesMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerServicePromiseClient.prototype.getIssues =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerService/GetIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_GetIssues);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.SortedIssueRequestMessage,
 *   !proto.IssueSummaryMessage>}
 */
const methodDescriptor_IssueTrackerService_GetAllIssues = new grpc.web.MethodDescriptor(
  '/IssueTrackerService/GetAllIssues',
  grpc.web.MethodType.SERVER_STREAMING,
  proto.SortedIssueRequestMessage,
  proto.IssueSummaryMessage,
  /**
   * @param {!proto.SortedIssueRequestMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.IssueSummaryMessage.deserializeBinary
);


/**
 * @param {!proto.SortedIssueRequestMessage} request The request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.IssueSummaryMessage>}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerServiceClient.prototype.getAllIssues =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/IssueTrackerService/GetAllIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_GetAllIssues);
};


/**
 * @param {!proto.SortedIssueRequestMessage} request The request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.IssueSummaryMessage>}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerServicePromiseClient.prototype.getAllIssues =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/IssueTrackerService/GetAllIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerService_GetAllIssues);
};


module.exports = proto;

