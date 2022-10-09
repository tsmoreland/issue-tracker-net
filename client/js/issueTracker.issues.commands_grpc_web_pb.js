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

var google_protobuf_empty_pb = require('google-protobuf/google/protobuf/empty_pb.js')
const proto = require('./issueTracker.issues.commands_pb.js');

/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?grpc.web.ClientOptions} options
 * @constructor
 * @struct
 * @final
 */
proto.IssueTrackerCommandServiceClient =
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
proto.IssueTrackerCommandServicePromiseClient =
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
 *   !proto.google.protobuf.Empty,
 *   !proto.google.protobuf.Empty>}
 */
const methodDescriptor_IssueTrackerCommandService_ResetDatabase = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/ResetDatabase',
  grpc.web.MethodType.UNARY,
  google_protobuf_empty_pb.Empty,
  google_protobuf_empty_pb.Empty,
  /**
   * @param {!proto.google.protobuf.Empty} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  google_protobuf_empty_pb.Empty.deserializeBinary
);


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.google.protobuf.Empty)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.google.protobuf.Empty>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.resetDatabase =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/ResetDatabase',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ResetDatabase,
      callback);
};


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.google.protobuf.Empty>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.resetDatabase =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/ResetDatabase',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ResetDatabase);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.AddIssueMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_AddIssue = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/AddIssue',
  grpc.web.MethodType.UNARY,
  proto.AddIssueMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.AddIssueMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
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
proto.IssueTrackerCommandServiceClient.prototype.addIssue =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/AddIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_AddIssue,
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
proto.IssueTrackerCommandServicePromiseClient.prototype.addIssue =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/AddIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_AddIssue);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.EditIssueMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_EditIssue = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/EditIssue',
  grpc.web.MethodType.UNARY,
  proto.EditIssueMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.EditIssueMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
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
proto.IssueTrackerCommandServiceClient.prototype.editIssue =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/EditIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_EditIssue,
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
proto.IssueTrackerCommandServicePromiseClient.prototype.editIssue =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/EditIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_EditIssue);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_DeleteIssue = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/DeleteIssue',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.deleteIssue =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/DeleteIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_DeleteIssue,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.deleteIssue =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/DeleteIssue',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_DeleteIssue);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_MoveToBackLogStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/MoveToBackLogStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.moveToBackLogStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/MoveToBackLogStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_MoveToBackLogStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.moveToBackLogStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/MoveToBackLogStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_MoveToBackLogStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_OpenStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/OpenStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.openStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/OpenStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_OpenStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.openStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/OpenStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_OpenStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_ReadyForReviewStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/ReadyForReviewStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.readyForReviewStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/ReadyForReviewStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ReadyForReviewStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.readyForReviewStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/ReadyForReviewStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ReadyForReviewStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_ReadyForTestStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/ReadyForTestStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.readyForTestStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/ReadyForTestStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ReadyForTestStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.readyForTestStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/ReadyForTestStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ReadyForTestStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_CompletedStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/CompletedStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.completedStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/CompletedStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_CompletedStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.completedStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/CompletedStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_CompletedStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_CloseStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/CloseStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.closeStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/CloseStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_CloseStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.closeStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/CloseStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_CloseStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_ReviewFailedStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/ReviewFailedStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.reviewFailedStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/ReviewFailedStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ReviewFailedStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.reviewFailedStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/ReviewFailedStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_ReviewFailedStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_TestFailedStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/TestFailedStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.testFailedStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/TestFailedStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_TestFailedStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.testFailedStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/TestFailedStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_TestFailedStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_CannotReproduceStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/CannotReproduceStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.cannotReproduceStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/CannotReproduceStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_CannotReproduceStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.cannotReproduceStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/CannotReproduceStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_CannotReproduceStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_WontDoStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/WontDoStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.wontDoStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/WontDoStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_WontDoStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.wontDoStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/WontDoStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_WontDoStateChange);
};


/**
 * @const
 * @type {!grpc.web.MethodDescriptor<
 *   !proto.IssueCommandMessage,
 *   !proto.StatusMessage>}
 */
const methodDescriptor_IssueTrackerCommandService_NotADefectStateChange = new grpc.web.MethodDescriptor(
  '/IssueTrackerCommandService/NotADefectStateChange',
  grpc.web.MethodType.UNARY,
  proto.IssueCommandMessage,
  issueTracker_issues_shared_pb.StatusMessage,
  /**
   * @param {!proto.IssueCommandMessage} request
   * @return {!Uint8Array}
   */
  function(request) {
    return request.serializeBinary();
  },
  issueTracker_issues_shared_pb.StatusMessage.deserializeBinary
);


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.RpcError, ?proto.StatusMessage)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.StatusMessage>|undefined}
 *     The XHR Node Readable Stream
 */
proto.IssueTrackerCommandServiceClient.prototype.notADefectStateChange =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/IssueTrackerCommandService/NotADefectStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_NotADefectStateChange,
      callback);
};


/**
 * @param {!proto.IssueCommandMessage} request The
 *     request proto
 * @param {?Object<string, string>=} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.StatusMessage>}
 *     Promise that resolves to the response
 */
proto.IssueTrackerCommandServicePromiseClient.prototype.notADefectStateChange =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/IssueTrackerCommandService/NotADefectStateChange',
      request,
      metadata || {},
      methodDescriptor_IssueTrackerCommandService_NotADefectStateChange);
};


module.exports = proto;

