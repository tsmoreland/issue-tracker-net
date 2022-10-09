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


var issueTracker_issues_shared_pb = require('./issueTracker.issues.shared_pb.js')
const proto = require('./issueTracker.issues.queries_pb.js');

/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?grpc.web.ClientOptions} options
 * @constructor
 * @struct
 * @final
 */
proto.IssueTrackerQueryServiceClient =
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
proto.IssueTrackerQueryServicePromiseClient =
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
 *   !proto.IssueByIdQueryMessage,
 *   !proto.IssueMessage>}
 */
const methodDescriptor_IssueTrackerQueryService_GetIssueById = new grpc.web.MethodDescriptor(
  '/IssueTrackerQueryService/GetIssueById',
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
proto.IssueTrackerQueryServiceClient.prototype.getIssueById =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerQueryService/GetIssueById',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerQueryService_GetIssueById,
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
proto.IssueTrackerQueryServicePromiseClient.prototype.getIssueById =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerQueryService/GetIssueById',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerQueryService_GetIssueById);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.PagedIssueRequestMessage,
 *   !proto.IssueSummariesMessage>}
 */
const methodDescriptor_IssueTrackerQueryService_GetIssues = new grpc.web.MethodDescriptor(
  '/IssueTrackerQueryService/GetIssues',
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
proto.IssueTrackerQueryServiceClient.prototype.getIssues =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerQueryService/GetIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerQueryService_GetIssues,
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
proto.IssueTrackerQueryServicePromiseClient.prototype.getIssues =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerQueryService/GetIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerQueryService_GetIssues);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueStreamRequestMessage,
 *   !proto.IssueSummaryMessage>}
 */
const methodDescriptor_IssueTrackerQueryService_GetAllIssues = new grpc.web.MethodDescriptor(
  '/IssueTrackerQueryService/GetAllIssues',
  grpc.web.MethodType.SERVER_STREAMING,
  proto.IssueStreamRequestMessage,
  proto.IssueSummaryMessage,
  /**
   * @param {!proto.IssueStreamRequestMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  proto.IssueSummaryMessage.deserializeBinary
);


/**
 * @param {!proto.IssueStreamRequestMessage} request The request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.IssueSummaryMessage>}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerQueryServiceClient.prototype.getAllIssues =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/IssueTrackerQueryService/GetAllIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerQueryService_GetAllIssues);
};


/**
 * @param {!proto.IssueStreamRequestMessage} request The request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.IssueSummaryMessage>}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerQueryServicePromiseClient.prototype.getAllIssues =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/IssueTrackerQueryService/GetAllIssues',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerQueryService_GetAllIssues);
};


module.exports = proto;

