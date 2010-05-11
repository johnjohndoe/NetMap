
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Enum: ClusterAlgorithm
//
/// <summary>
/// Specifies the algorithm used to partition a graph into clusters.
/// </summary>
//*****************************************************************************

public enum
ClusterAlgorithm
{
    // Note:
    //
    // These values get stored in user setting files and their numerical
    // values should not be modified.

    /// <summary>
    /// Use the Wakita-Tsurumi cluster algorithm.
    /// </summary>
    ///
    /// <remarks>
    /// The algorithm is described in "Finding Community Structure in
    /// Mega-scale Social Networks," by Ken Wakita and Toshiyuki Tsurumi.  The
    /// paper can be found here:
    ///
    /// <para>
    /// http://arxiv.org/PS_cache/cs/pdf/0702/0702048v1.pdf
    /// </para>
    ///
    /// <para>
    /// NodeXL implements the data structure described in section 4.1 of the
    /// paper.  It does not implement the heuristics described in section 4.2.
    /// </para>
    ///
    /// </remarks>

    WakitaTsurumi,

    /// <summary>
    /// Use the Girvan-Newman cluster algorithm.
    /// </summary>
    ///
    /// <remarks>
    /// The algorithm is described in "Community Structure in Social and
    /// Biological Networks," by Michelle Girvan and M. E. J. Newman.  The
    /// paper can be found here:
    ///
    /// <para>
    /// http://www.santafe.edu/media/workingpapers/01-12-077.pdf
    /// </para>
    ///
    /// <para>
    /// This algorithm is implemented by the SNAP graph library.
    /// </para>
    ///
    /// </remarks>

    GirvanNewman,

    /// <summary>
    /// Use the Clauset-Newman-Moore algorithm.
    /// </summary>
    ///
    /// <remarks>
    /// The algorithm is descibed in "Finding Community Structure in Very Large
    /// Networks," by Aaron Clauset, M. E. J. Newman, and Cristopher Moore.
    /// The paper can be found here:
    ///
    /// <para>
    /// http://www.ece.unm.edu/ifis/papers/community-moore.pdf
    /// </para>
    ///
    /// <para>
    /// This algorithm is implemented by the SNAP graph library.
    /// </para>
    ///
    /// </remarks>
    
    ClausetNewmanMoore,
}
}
