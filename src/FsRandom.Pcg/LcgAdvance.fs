// Copyright (c) 2017 Theodore Tsirpanis
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace FsRandom

/// Contains functions that effeciently advance LCGs.
module LcgAdvance =

    open SoftWx.Numerics

    /// Efficiently calculates `a ^ exp mod 2 ^ 64`.
    let rec modExp64 a exp =
        if exp = 0UL then
            1UL
        elif exp &&& 1UL = 0UL then
            let x = exp >>> 1 |> modExp64 a
            x * x
        else
            exp - 1UL |> modExp64 a |> ((*) a)

    /// Efficiently calculates `a ^ exp mod 2 ^ 128`.
    let rec modExp128 a exp =
        let two = UInt128.One + UInt128.One
        if exp = UInt128.Zero then
            UInt128.One
        elif exp.Low % 2UL = 0UL then
            let x = exp >>> 1 |> modExp128 a
            x * x
        else
            exp - UInt128.One |> modExp128 a |> ((*) a)

    /// Efficiently advances a 64-bit LCG state with multiplier `a`, incrementer `b`, by `delta` steps.
    let advance64 state delta a b =
        let an = modExp64 a delta
        an * state + (an - 1UL) * b / (a - 1UL)

    /// Efficiently advances a 128-bit LCG state with multiplier `a`, incrementer `b`, by `delta` steps.
    let advance128 state delta a b =
        let an = modExp128 a delta
        an * state + (an - UInt128.One) * b / (a - UInt128.One)
